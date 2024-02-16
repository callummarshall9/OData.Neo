//-----------------------------------------------------------------------
// Copyright (c) .NET Foundation and Contributors. All rights reserved.
// See License.txt in the project root for license information.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using FluentAssertions;
using OData.Neo.Core.Models.OTokens;
using OData.Neo.Core.Models.ProjectedTokens;
using Xunit;

namespace OData.Neo.Core.Tests.Unit.Services.Foundations.OTokenizations
{
    public partial class OTokenizationServiceTests
    {
        [Fact]
        public void ShouldOTokenizeSelect()
        {
            // given
            var unidentifiedOTokens = new OToken[]
            {
                new OToken
                {
                    ProjectedType = ProjectedTokenType.Keyword,
                    RawValue = "$select",
                },

                new OToken
                {
                    ProjectedType = ProjectedTokenType.Equals,
                    RawValue = "=",
                },

                new OToken
                {
                    ProjectedType = ProjectedTokenType.Property,
                    RawValue = "Name",
                }
            };

            OToken[] inputTokens = unidentifiedOTokens;

            OToken expectedToken = new OToken
            {
                Type = OTokenType.Root,

                Children = new List<OToken>
                {
                    new OToken
                    {
                        RawValue = "$select",
                        Type = OTokenType.Select,
                        ProjectedType = ProjectedTokenType.Keyword,

                        Children = new List<OToken>
                        {
                            new OToken
                            {
                                ProjectedType = ProjectedTokenType.Property,
                                RawValue = "Name",
                                Type = OTokenType.Property
                            }
                        }
                    }
                }
            };

            // when
            OToken actualToken =
                this.tokenizationService.OTokenize(inputTokens);

            // then
            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [Fact]
        public void ShouldOTokenizeExpand()
        {
            // given
            var unidentifiedOTokens = new OToken[]
            {
                new OToken
                {
                    ProjectedType = ProjectedTokenType.Keyword,
                    RawValue = "$expand",
                },

                new OToken
                {
                    ProjectedType = ProjectedTokenType.Equals,
                    RawValue = "=",
                },

                new OToken
                {
                    ProjectedType = ProjectedTokenType.Property,
                    RawValue = "Names",
                }
            };

            OToken[] inputTokens = unidentifiedOTokens;

            OToken expectedToken = new OToken
            {
                Type = OTokenType.Root,

                Children = new List<OToken>
                {
                    new OToken
                    {
                        RawValue = "$expand",
                        Type = OTokenType.Expand,
                        ProjectedType = ProjectedTokenType.Keyword,

                        Children = new List<OToken>
                        {
                            new OToken
                            {
                                ProjectedType = ProjectedTokenType.Property,
                                RawValue = "Names",
                                Type = OTokenType.Property
                            }
                        }
                    }
                }
            };

            // when
            OToken actualToken =
                this.tokenizationService.OTokenize(inputTokens);

            // then
            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [Fact]
        public void ShouldOTokenizeSelectExpand()
        {
            // given
            var unidentifiedOTokens = new OToken[]
            {
                new OToken
                {
                    ProjectedType = ProjectedTokenType.Keyword,
                    RawValue = "$expand",
                },
                new OToken
                {
                    ProjectedType = ProjectedTokenType.Equals,
                    RawValue = "=",
                },
                new OToken
                {
                    ProjectedType = ProjectedTokenType.Property,
                    RawValue = "LibraryCards",
                },
                new OToken
                {
                    ProjectedType = ProjectedTokenType.Brackets,
                    RawValue = "(",
                },
                new OToken
                {
                    ProjectedType = ProjectedTokenType.Keyword,
                    RawValue = "$select",
                },
                new OToken
                {
                    ProjectedType = ProjectedTokenType.Equals,
                    RawValue = "=",
                },
                new OToken
                {
                    ProjectedType = ProjectedTokenType.Property,
                    RawValue = "Name",
                },
                new OToken
                {
                    ProjectedType = ProjectedTokenType.Brackets,
                    RawValue = ")",
                },
            };

            OToken[] inputTokens = unidentifiedOTokens;

            OToken expectedToken = new OToken
            {
                Type = OTokenType.Root,

                Children = new List<OToken>
                {
                    new OToken
                    {
                        RawValue = "$expand",
                        Type = OTokenType.Expand,
                        ProjectedType = ProjectedTokenType.Keyword,

                        Children = new List<OToken>
                        {
                            new OToken
                            {
                                ProjectedType = ProjectedTokenType.Property,
                                RawValue = "LibaryCards",
                                Type = OTokenType.Property,
                                Children = new List<OToken>
                                {
                                    new OToken
                                    {
                                        RawValue = "$select",
                                        Type = OTokenType.Select,
                                        ProjectedType = ProjectedTokenType.Keyword,
                                        Children = new List<OToken>
                                        {
                                            new OToken
                                            {
                                                RawValue = "Name",
                                                Type = OTokenType.Property,
                                                ProjectedType = ProjectedTokenType.Property,
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // when
            OToken actualToken =
                this.tokenizationService.OTokenize(inputTokens);

            // then
            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [Fact]
        public void ShouldOTokenizeEmptyArrays()
        {
            // given
            var unidentifiedOTokens = Array.Empty<OToken>();

            OToken[] inputTokens = unidentifiedOTokens;

            OToken expectedToken = new OToken
            {
                Type = OTokenType.Root,
                Children = new List<OToken>()
            };

            // when
            OToken actualToken =
                this.tokenizationService.OTokenize(inputTokens);

            // then
            actualToken.Should().BeEquivalentTo(expectedToken);
        }
    }
}