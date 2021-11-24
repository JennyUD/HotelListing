using AutoMapper;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger <CountryController> _logger;
        private readonly IMapper _mapper;
           public CountryController (IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
           {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            }

        [HttpGet] 
        public async Task <IActionResult> GetCountries()
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetAll();
               var results = _mapper.Map<IList<CountryDTO>>(countries);
                                return Ok(results);
            }
            catch (Exception ex)
            {
           _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountries)}");
                return StatusCode(500, "Internal Server error. Try again");
            }

        }

        [HttpGet("{id:int}", Name ="GetCountry")]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await _unitOfWork.Countries.Get(q =>
                q.ID == id, new List<string> { "Hotels" });
                var results = _mapper.Map<CountryDTO>(country);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountry)}");
                return StatusCode(500, "Internal Server error. Try again");
            }

        }


        [HttpPost
           //  , Authorize(Roles ="Administrator")
           ]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO CountryDTO)
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError($"Invalid POST attempt in {nameof(CreateCountry)}");
                    return BadRequest(ModelState);
                }
                try
                {
                    var country = _mapper.Map<Country>(CountryDTO);
                    await _unitOfWork.Countries.Insert(country);
                    await _unitOfWork.Save();
                    return CreatedAtRoute("GetCountry", new { id = country.ID }, country);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Something Went in the {nameof(GetCountries)}");
                    return StatusCode(500, "Internal Server Error. Please try again");
                }
            }


        [HttpPut("{id:int}")
          ]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(
            int id
            , [FromBody] UpdateCountryDTO CountryDTO
          )
        {
            if (ModelState.IsValid == false || id < 1)
            {
                _logger.LogError($"Invalid Update attempt in {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }
            try
            {
                var GETCountry = await _unitOfWork.Countries.Get(q => q.ID == id);
               // var GETHotel = await _unitOfWork.Hotels.Get(n=>n. CountryId==id);
                if (GETCountry == null)
                {
                    _logger.LogError($"Invalid Request in {nameof(UpdateCountry)}");
                    return BadRequest("Submitted data invalid");
                }
                _mapper.Map(CountryDTO, GETCountry);
              //  _mapper.Map(CountryDTO, GETHotel);
                _unitOfWork.Countries.Update(GETCountry);
               // _unitOfWork.Hotels.Update(GETHotel);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateCountry)}");
                return StatusCode(500, "Internal Server error. Try again");
            }
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
                return BadRequest();
            }
            try
            {
                var country = await _unitOfWork.Hotels.Get(q => q.ID == id);
                if (country == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
                    return BadRequest("Submitted data is not valid");
                }
                await _unitOfWork.Countries.Delete(id);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteCountry)}");
                return StatusCode(500, "Internal Server Error. Please try later");
            }
        }











    }

}
