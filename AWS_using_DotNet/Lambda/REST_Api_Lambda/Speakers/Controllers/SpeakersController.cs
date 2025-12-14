using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Speakers.Models;
using System.ComponentModel;

namespace Speakers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeakersController : ControllerBase
    {
        private readonly SpeakerDataService _speakerDataService;
        private readonly ILogger<SpeakersController> _logger;

        public SpeakersController(SpeakerDataService speakerDataService, ILogger<SpeakersController> logger)
        {
            _speakerDataService = speakerDataService;
            _logger = logger;
        }


        [HttpGet]
        [Route("getSpeakers")]
        public IActionResult GetSpeakers()
        {
            _logger.LogInformation("GetSpeakers called");
            return Ok(_speakerDataService.SpeakersList);
        }

        [HttpGet]
        [Route("getSpeakerById")]
        public IActionResult GetSpeaker(int id)
        {
            _logger.LogInformation("GetSpeakerById called with id: {SpeakerId}", id);
            return Ok(_speakerDataService.SpeakersList.Where(x => x.Id == id).FirstOrDefault() ?? new Speaker());
        }

        [HttpPost]
        [Route("searchSpeaker")]
        public IActionResult SearchSpeaker(string searchTerm)
        {
            _logger.LogInformation("SearchSpeakerBySearchTerm called with search term: {SearchTerm}", searchTerm);
            return Ok(_speakerDataService.SpeakersList.Where(x => searchTerm.Contains(x.Bio)).ToList());
        }

        [HttpPost]
        [Route("addSpeaker")]
        public IActionResult AddSpeaker([FromBody] Speaker newSpeaker)
        {
            if (newSpeaker == null)
            {
                _logger.LogWarning("AddSpeaker called with null speaker");
                return BadRequest("Speaker cannot be null");
            }
            _speakerDataService.SpeakersList.Add(newSpeaker);
            _logger.LogInformation("New speaker added: {SpeakerName}", newSpeaker.Name);
            return Ok(newSpeaker);
        }

        [HttpPost]
        [Route("deleteSpeaker")]
        public IActionResult DeleteSpeaker(int id)
        {
            var speaker = _speakerDataService.SpeakersList.FirstOrDefault(x => x.Id == id);
            if (speaker == null)
            {
                _logger.LogWarning("DeleteSpeaker called with invalid id: {SpeakerId}", id);
                return NotFound("Speaker not found");
            }
            _speakerDataService.SpeakersList.Remove(speaker);
            _logger.LogInformation("Speaker deleted: {SpeakerName}", speaker.Name);
            return Ok();
        }
    }
}
