//-----------------------------------------------------------------------
// Copyright (c) .NET Foundation and Contributors. All rights reserved.
// See License.txt in the project root for license information.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using OData.Neo.Core.Models.OTokens;
using OData.Neo.Core.Models.ProjectedTokens;

namespace OData.Neo.Core.Services.Foundations.OTokenizations
{
    public partial class OTokenizationService : IOTokenizationService
    {
        private readonly IOTokenizationValidationService oTokenizationValidationService;

        public OTokenizationService(IOTokenizationValidationService oTokenizationValidationService)
        {
            this.oTokenizationValidationService = oTokenizationValidationService;
        }

        public OToken OTokenize(OToken[] oTokens) =>
            TryCatch(() =>
            {
                oTokenizationValidationService.ValidateOTokens(oTokens);

                OToken root = new()
                {
                    Type = OTokenType.Root,
                    Children = new List<OToken>()
                };

                return oTokens.Any()
                    ? ProcessTokens(root, oTokens)
                    : root;
            });

        OTokenType GetKeywordTokenType(OToken token)
            => token.RawValue switch
            {
                "$select" => OTokenType.Select,
                "$expand" => OTokenType.Expand,
                _ => OTokenType.Unidentified
            };

        OToken ProcessTokens(OToken root, OToken[] oTokens)
        {
            OToken rootNode = root;

            OToken currentRoot = rootNode;

            currentRoot.Children ??= new List<OToken>();

            foreach(var token in oTokens)
            {
                if (token.ProjectedType == ProjectedTokenType.Brackets && currentRoot.Type == OTokenType.Expand && currentRoot.Children.Any())
                {
                    var newRootNode = currentRoot.Children.Last();

                    newRootNode.Parent = currentRoot;
                    newRootNode.Children ??= new List<OToken>();

                    currentRoot = newRootNode;//$expand=LibraryCards($select=Name), so LibraryCards.
                }
                else if (token.ProjectedType == ProjectedTokenType.Brackets && currentRoot.Type != OTokenType.Expand)
                    currentRoot = currentRoot.Parent;
                else if (token.ProjectedType == ProjectedTokenType.Space)
                    continue;
                else if (token.ProjectedType == ProjectedTokenType.Equals)
                    continue;
                else if (token.ProjectedType == ProjectedTokenType.Comma)
                    continue;
                else if (token.ProjectedType == ProjectedTokenType.Keyword)
                {
                    var newKeywordToken = new OToken
                    {
                        Type = GetKeywordTokenType(token),
                        ProjectedType = token.ProjectedType,
                        Children = new List<OToken>(),
                        Parent = currentRoot,
                        RawValue = token.RawValue
                    };

                    currentRoot.Children.Add(newKeywordToken);

                    currentRoot = newKeywordToken;
                } 
                else
                {
                    currentRoot.Children.Add(new OToken
                    {
                        Type = OTokenType.Property,
                        ProjectedType = token.ProjectedType,
                        Parent = currentRoot,
                        RawValue = token.RawValue,
                    });
                }
            }

            NullParent(new[] { root });

            return root;
        }

        void NullParent(IEnumerable<OToken> children)
        {
            children ??= new List<OToken>();

            foreach(var entry in children)
            {
                entry.Parent = null;
                NullParent(entry.Children);
            }
        }
    }
}
