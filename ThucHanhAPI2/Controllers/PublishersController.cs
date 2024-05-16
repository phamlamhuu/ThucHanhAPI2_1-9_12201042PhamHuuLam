﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThucHanhAPI2.Data;
using ThucHanhAPI2.Model.Domain;
using ThucHanhAPI2.Model.DTO;
using ThucHanhAPI2.Respository;

namespace ThucHanhAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IPublisherRepository _publisherRepository;
        public PublishersController(AppDbContext dbContext, IPublisherRepository
       publisherRepository)
        {
            _dbContext = dbContext;
            _publisherRepository = publisherRepository;
        }
        [HttpGet("get-all-publisher")]
        public IActionResult GetAllPublisher()
        {
            var allPublishers = _publisherRepository.GetAllPublishers();
            return Ok(allPublishers);
        }
        [HttpGet("get-publisher-by-id")]
        public IActionResult GetPublisherById(int id)
        {
            var publisherWithId = _publisherRepository.GetPublisherById(id);
            return Ok(publisherWithId);
        }
        [HttpPost("add - publisher")]
        public IActionResult AddPublisher([FromBody] AddPublisherRequestDTO
       addPublisherRequestDTO)
        {
            var publisherAdd = _publisherRepository.AddPublisher(addPublisherRequestDTO);
            return Ok(publisherAdd);
        }
        [HttpPut("update-publisher-by-id/{id}")]
        public IActionResult UpdatePublisherById(int id, [FromBody] PublisherNoIdDTO
       publisherDTO)
        {
            var publisherUpdate = _publisherRepository.UpdatePublisherById(id,
           publisherDTO);

            return Ok(publisherUpdate);
        }
        [HttpDelete("delete-publisher-by-id/{id}")]
        public IActionResult DeletePublisherById(int id)
        {
            var publisherDelete = _publisherRepository.DeletePublisherById(id);
            return Ok();
        }
    }
}
