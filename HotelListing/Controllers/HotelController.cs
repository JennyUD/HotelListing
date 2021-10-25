using AutoMapper;
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
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _IUnitOfWork;
        private readonly ILogger<HotelController> _ILogger;
        private readonly IMapper _IMapper;

        public HotelController(IUnitOfWork iUnitOfWork,  ILogger<HotelController> iLogger,IMapper iMapper)
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



        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAHotel(int id)
        {
            try
            {
                var Hotel = await _IUnitOfWork.Hotels.Get(n=>
                n.ID==id, new List<string>{ "Country"});
                var results = _IMapper.Map<HotelDTO>(Hotel);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _ILogger.LogError(ex, $"Something went wrong in the {nameof(GetHotels)}");
                return StatusCode(500, "Internal Server error. Try again");
            }
        }






    }

}
