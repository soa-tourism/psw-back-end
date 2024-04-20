using Explorer.Stakeholders.API.Dtos;

namespace Explorer.Stakeholders.Core.Mappers
{
    public class ObjectRequestMapper
    {
        public ObjectRequestMapper() { }

        public ObjectRequestDto createDto(int objectId, int authorId, string status)
        {
            ObjectRequestDto objectRequestDto = new ObjectRequestDto();
            objectRequestDto.MapObjectId = objectId;
            objectRequestDto.AuthorId = authorId;
            objectRequestDto.Status = status;

            return objectRequestDto;

        }
    }
}
