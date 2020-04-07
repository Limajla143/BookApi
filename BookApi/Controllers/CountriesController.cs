﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApi.Dtos;
using BookApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : Controller
    {
        private ICountryRepository countryRepository { get; set; }

        public CountriesController(ICountryRepository _countryRepository)
        {
            countryRepository = _countryRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        // api/countries
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CountryDto>))]
        public IActionResult GetCountries()
        {
            var countries = countryRepository.GetCountries().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryDto = new List<CountryDto>();

            foreach(var country in countries)
            {
                countryDto.Add(new CountryDto
                    {
                    Id = country.Id,
                    Name = country.Name
                });
            }

            return Ok(countryDto);
        }

        // api/countries/countryId
        [HttpGet("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        public IActionResult GetCountry(int countryId)
        {
            if (!countryRepository.CountryExist(countryId))
                return NotFound();

            var country = countryRepository.GetCountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryDto = new CountryDto()
            {
                Id = country.Id,
                Name = country.Name
            };

            return Ok(countryDto);
        }

        [HttpGet("author/{authorId}/country")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        public IActionResult GetCountryOfAuthor(int authorId)
        {
            //Validate author exists

            var country = countryRepository.GetCountryOfAnAuthor(authorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryDto = new CountryDto()
            {
                Id = country.Id,
                Name = country.Name
            };

            return Ok(countryDto);
        }


        //authors
        [HttpGet("{countryId}/authors")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDto>))]
        public IActionResult GetAuthorsFromCountry(int countryId)
        {
            if (!countryRepository.CountryExist(countryId))
                return NotFound();

            var authors = countryRepository.GetAuthorsFromCountry(countryId).ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorDtos = new List<AuthorDto>();

            foreach (var author in authors)
            {
                authorDtos.Add(new AuthorDto
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                    
                });
            }

            return Ok(authorDtos);
        }
    }
}