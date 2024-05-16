using ThucHanhAPI2.Model.Domain;
using ThucHanhAPI2.Model.DTO;

namespace ThucHanhAPI2.Respository
{
    public interface IPublisherRepository
    {
        List<PublisherDTO> GetAllPublishers();
        PublisherNoIdDTO GetPublisherById(int id);
        AddPublisherRequestDTO AddPublisher(AddPublisherRequestDTO addPublisherRequestDTO);
        PublisherNoIdDTO UpdatePublisherById(int id, PublisherNoIdDTO publisherNoIdDTO);
        Publisher? DeletePublisherById(int id);
    }
}
