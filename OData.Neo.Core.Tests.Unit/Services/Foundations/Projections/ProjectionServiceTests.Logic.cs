//-----------------------------------------------------------------------
// Copyright (c) .NET Foundation and Contributors. All rights reserved.
// See License.txt in the project root for license information.
//-----------------------------------------------------------------------

using FluentAssertions;
using OData.Neo.Core.Models.ProjectedTokens;
using OData.Neo.Core.Models.Tokens;
using Xunit;

namespace OData.Neo.Core.Tests.Unit.Services.Foundations.Projections
{
    public partial class ProjectionServiceTests
    {
        [Fact]
        public void ShouldProjectNameQuery()
        {
            //given
            var inputProjectedTokens = new ProjectedToken[]
            {
                new ProjectedToken
                {
                    RawValue = "$filter",
                    TokenType = TokenType.Word,
                    ProjectedType = ProjectedTokenType.Unidentified
                },
                new ProjectedToken
                {
                    RawValue = "=",
                    TokenType = TokenType.Separator,
                    ProjectedType = ProjectedTokenType.Unidentified
                },
               new ProjectedToken
                {
                    RawValue = "Name",
                    TokenType = TokenType.Word,
                    ProjectedType = ProjectedTokenType.Unidentified
                },
               new ProjectedToken
                {
                    RawValue = " ",
                    TokenType = TokenType.Separator,
                    ProjectedType = ProjectedTokenType.Unidentified
                },
                new ProjectedToken
                {
                    RawValue = "'",
                    TokenType = TokenType.Separator,
                    ProjectedType = ProjectedTokenType.Unidentified
                },
                new ProjectedToken
                {
                    RawValue = "Foo",
                    TokenType = TokenType.Word,
                    ProjectedType = ProjectedTokenType.Unidentified
                },
                new ProjectedToken
                {
                    RawValue = "'",
                    TokenType = TokenType.Separator,
                    ProjectedType = ProjectedTokenType.Unidentified
                }
            };

            var expectedProjectedTokens = new ProjectedToken[]
           {
                new ProjectedToken
                {
                    RawValue = "$filter",
                    TokenType = TokenType.Word,
                    ProjectedType = ProjectedTokenType.Keyword
                },
                new ProjectedToken
                {
                    RawValue = "=",
                    TokenType = TokenType.Separator,
                    ProjectedType = ProjectedTokenType.Assignment
                },
               new ProjectedToken
                {
                    RawValue = "Name",
                    TokenType = TokenType.Word,
                    ProjectedType = ProjectedTokenType.Property
                },
               new ProjectedToken
                {
                    RawValue = " ",
                    TokenType = TokenType.Separator,
                    ProjectedType = ProjectedTokenType.Space
                },
                new ProjectedToken
                {
                    RawValue = "'",
                    TokenType = TokenType.Separator,
                    ProjectedType = ProjectedTokenType.Quotes
                },
                new ProjectedToken
                {
                    RawValue = "Foo",
                    TokenType = TokenType.Word,
                    ProjectedType = ProjectedTokenType.Property
                },
                new ProjectedToken
                {
                    RawValue = "'",
                    TokenType = TokenType.Separator,
                    ProjectedType = ProjectedTokenType.Quotes
                }
           };

            // when
            ProjectedToken[] actualProjectedTokens =
                this.projectionService.ProjectTokens(
                    inputProjectedTokens);

            // then
            actualProjectedTokens.Should().BeEquivalentTo(expectedProjectedTokens);
        }

        [Fact]
        public void ShouldProjectKeywords()
        {
            // given
            var inputProjectedTokens = new ProjectedToken[]
            {
                new ProjectedToken
                {
                    RawValue = "$filter",
                    TokenType = TokenType.Word,
                    ProjectedType = ProjectedTokenType.Unidentified
                }
            };

            var expectedProjectedTokens = new ProjectedToken[]
            {
                new ProjectedToken
                {
                    RawValue = "$filter",
                    TokenType = TokenType.Word,
                    ProjectedType = ProjectedTokenType.Keyword
                }
            };

            // when
            ProjectedToken[] actualProjectedTokens =
                this.projectionService.ProjectTokens(
                    inputProjectedTokens);

            // then
            actualProjectedTokens.Should().BeEquivalentTo(expectedProjectedTokens);
        }

        [Theory]
        [MemberData(nameof(ProjectedTokens))]
        public void ShouldProjectAllTokensKeywords(
            ProjectedToken[] inputProjectedTokens,
            ProjectedToken[] expectedProjectedTokens)
        {
            // given . when
            ProjectedToken[] actualProjectedTokens =
                this.projectionService.ProjectTokens(
                    inputProjectedTokens);

            // then
            actualProjectedTokens.Should().BeEquivalentTo(
                expectedProjectedTokens);
        }
    }
}
