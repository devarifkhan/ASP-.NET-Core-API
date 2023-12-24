using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //private field
        private readonly List<Country> _countries;

        //constructor
        public CountriesService(bool initialize =true)
        {
            _countries = new List<Country>();
            if (initialize)
            {
                _countries.AddRange(new List<Country>() {

                new Country()
                {
                    CountryID=Guid.Parse("ACABC46A-E945-4408-A9A1-37D4C598D421"),
                    CountryName="Bangladesh"
                },
                new Country()
                {
                    CountryID = Guid.Parse("ACF3F31A-F9F5-4621-A2D6-F60846EFD19A"),
                    CountryName = "USA"
                },
                new Country()
                {
                    CountryID = Guid.Parse("FAB6CF84-FCFF-4541-BAA3-968F8C1DC45A"),
                    CountryName = "France"
                },
                new Country()
                {
                    CountryID = Guid.Parse("5DB51CD2-2224-4753-B0E8-25FE3478B812"),
                    CountryName = "Italy"
                },
                new Country()
                {
                    CountryID = Guid.Parse("6ABE0604-6251-4700-B466-6D33CBCA188C"),
                    CountryName = "Germany"
                }



                });
                
           
            }
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {

            //Validation: countryAddRequest parameter can't be null
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            //Validation: CountryName can't be null
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            //Validation: CountryName can't be duplicate
            if (_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
            {
                throw new ArgumentException("Given country name already exists");
            }

            //Convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();

            //generate CountryID
            country.CountryID = Guid.NewGuid();

            //Add country object into _countries
            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null)
                return null;

            Country? country_response_from_list = _countries.FirstOrDefault(temp => temp.CountryID == countryID);

            if (country_response_from_list == null)
                return null;

            return country_response_from_list.ToCountryResponse();
        }
    }
}
