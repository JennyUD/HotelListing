using AutoMapper;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelListing.Data;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _IUnitOfWork;
        private readonly ILogger<HotelController> _ILogger;
        private readonly IMapper _IMapper;

        public HotelController(IUnitOfWork iUnitOfWork, ILogger<HotelController> iLogger, IMapper iMapper)
        {
            _IUnitOfWork = iUnitOfWork;
            _ILogger = iLogger;
            _IMapper = iMapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var Hotels = await _IUnitOfWork.Hotels.GetAll();
                var results = _IMapper.Map<IList<HotelDTO>>(Hotels);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _ILogger.LogError(ex, $"Something went wrong in the {nameof(GetHotels)}");
                return StatusCode(500, "Internal Server error. Try again");
            }
        }


        [HttpGet("{id:int}", Name = "GetAHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //  [Authorize]
        public async Task<IActionResult> GetAHotel(int id)
        {
            try
            {

                var Hotel = await _IUnitOfWork.Hotels.Get(n =>
                n.ID == id, new List<string> { "Country" });
                var results = _IMapper.Map<HotelDTO>(Hotel);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _ILogger.LogError(ex, $"Something went wrong in the {nameof(GetAHotel)}");
                return StatusCode(500, "Internal Server error. Try again");
            }
        }


        [HttpPost
       //  , Authorize(Roles ="Administrator")
       ]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel ([FromBody] CreateHotelDTO HotelDTO)
        {
            if (!ModelState.IsValid)
            {
                _ILogger.LogError($"Invalid POST attempt in {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }
            try
            {
                var hotel = _IMapper.Map<Hotel>(HotelDTO);
                await _IUnitOfWork.Hotels.Insert(hotel);
                await _IUnitOfWork.Save();
                return CreatedAtRoute("GetAHotel", new { id = hotel.ID }, hotel);
            }
            catch (Exception ex)
            {
                _ILogger.LogError(ex, $"Something Went in the {nameof(GetHotels)}");
                return StatusCode(500, "Internal Server Error. Please try again");
            }
        }



        [HttpPut ("{id:int}")
            ]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel (int id, [FromBody] UpdateHotelDTO HotelDTO
            )
        {
            if (ModelState.IsValid == false || id < 1)
            {
                _ILogger.LogError($"Invalid Update attempt in {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }
            try
            {
                var GETHotel = await _IUnitOfWork.Hotels.Get(q => q.ID == id);
                if (GETHotel == null)
                {
                    _ILogger.LogError($"Invalid Request in {nameof(UpdateHotel)}");
                    return BadRequest("Submitted data invalid");
                }
                _IMapper.Map(HotelDTO, GETHotel);
                _IUnitOfWork.Hotels.Update(GETHotel);
                await _IUnitOfWork.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _ILogger.LogError(ex, $"Something went wrong in the {nameof(UpdateHotel)}");
                return StatusCode(500, "Internal Server error. Try again");
            }
        }
        
       
       // [Authorize]
       [HttpDelete("{id:int}")]
       [ProducesResponseType(StatusCodes.Status400BadRequest)]
       [ProducesResponseType(StatusCodes.Status204NoContent)]
       [ProducesResponseType(StatusCodes.Status500InternalServerError)]
       
       public async Task<IActionResult> DeleteHotel(int id)
        {
            if (id < 1)
            {
                _ILogger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
                return BadRequest();
            }
            try
            {
                var hotel = await _IUnitOfWork.Hotels.Get(q => q.ID == id);
                if (hotel == null)
                {
                    _ILogger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
                    return BadRequest("Submitted data is not valid");
                }
                await _IUnitOfWork.Hotels.Delete(id);
                await _IUnitOfWork.Save();
                return NoContent();
            }
            catch(Exception ex)
            {
                _ILogger.LogError(ex, $"Something went wrong in the {nameof(DeleteHotel)}");
                return StatusCode(500, "Internal Server Error. Please try later");
            }
        }
    
















    }

}
  