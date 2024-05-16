using Microsoft.EntityFrameworkCore;
using ThucHanhAPI2.Data;
using ThucHanhAPI2.Respository;
using ThucHanhAPI2.Model.Domain;
using ThucHanhAPI2.Model.DTO;
namespace ThucHanhAPI2.Repositories
{
    public class SQLPublisherRepository : IPublisherRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLPublisherRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<PublisherDTO> GetAllPublishers()
        {
            var allPublishersDomain = _dbContext.Publisher.ToList();
            var allPublisherDTO = new List<PublisherDTO>();
            foreach (var publisherDomain in allPublishersDomain)
            {
                allPublisherDTO.Add(new PublisherDTO()
                {
                    Id = publisherDomain.Id,
                    Name = publisherDomain.Name
                });
            }
            return allPublisherDTO;
        }
        public PublisherNoIdDTO GetPublisherById(int id)
        {
            var publisherWithIdDomain = _dbContext.Publisher.FirstOrDefault(x => x.Id ==
           id);
            if (publisherWithIdDomain != null)
            {
                var publisherNoIdDTO = new PublisherNoIdDTO
                {
                    Name = publisherWithIdDomain.Name,
                };
                return publisherNoIdDTO;
            }
            return null;
        }
        public AddPublisherRequestDTO AddPublisher(AddPublisherRequestDTO
       addPublisherRequestDTO)
        {
            var publisherDomainModel = new Publisher
            {
                Name = addPublisherRequestDTO.Name,
            };
            _dbContext.Publisher.Add(publisherDomainModel);
            _dbContext.SaveChanges();
            return addPublisherRequestDTO;
        }
        public PublisherNoIdDTO UpdatePublisherById(int id, PublisherNoIdDTO
       publisherNoIdDTO)
        {
            var publisherDomain = _dbContext.Publisher.FirstOrDefault(n => n.Id == id);
            if (publisherDomain != null)
            {
                publisherDomain.Name = publisherNoIdDTO.Name;
                _dbContext.SaveChanges();
            }
            return null;
        }
        public Publisher? DeletePublisherById(int id)
        {
            var publisherDomain = _dbContext.Publisher.FirstOrDefault(n => n.Id == id);
            if (publisherDomain != null)
            {
                _dbContext.Publisher.Remove(publisherDomain);
                _dbContext.SaveChanges();
            }
            return null;
        }
    }
}
