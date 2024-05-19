﻿using MediatR;
using Postech.PhaseOne.GroupEight.TechChallenge.Domain.Commands.Outputs;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Postech.PhaseOne.GroupEight.TechChallenge.Domain.Commands.Inputs
{
    /// <summary>
    /// Object that stores contact search data.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FindContactInput : IRequest<DefaultOutput>
    {
        /// <summary>
        /// The area code of the contact's phone number that will be used as a filter for the search.
        /// </summary>
        [Required(ErrorMessage = "Contact area code is required.")]
        [MaxLength(2, ErrorMessage = "The area code of the contact's phone number must contain a maximum of two digits.")]
        public string AreaCodeValue { get; init; }
    }
}