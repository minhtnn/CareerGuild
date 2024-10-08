using CareerGuidance.Domain;

namespace CareerGuidance.Repository;

public interface ICareerService
{
    Career GetCareer(string url, ICollection<string> information);
}