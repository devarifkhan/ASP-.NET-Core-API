using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;


namespace CRUD_API.Controllers
{
    [Route("[controller]")]
    public class PersonsController : Controller
    {
        //private fields

        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;


        //contructor

        public PersonsController(IPersonsService personsService, ICountriesService countriesService)
        {
            _personsService = personsService;
            _countriesService = countriesService;
        }

        [Route("[action]")]
        [Route("/")]
        public IActionResult Index(string searchBy,string? searchString, string sortBy=nameof(PersonResponse.PersonName),SortOrderOptions sortOrder=SortOrderOptions.ASC)
        {
            //Search
            ViewBag.SearchFields = new Dictionary<string, string>() {
                { nameof(PersonResponse.PersonName),"Person Name" },
                { nameof(PersonResponse.Email),"Email" },
                { nameof(PersonResponse.DateOfBirth),"Date of Birth" },
                { nameof(PersonResponse.Gender),"Gender" },
                { nameof(PersonResponse.CountryID),"Country" },
                { nameof(PersonResponse.Address),"Address" },
               



            };
           List<PersonResponse> persons=   _personsService.GetFilteredPersons(searchBy,searchString );
            ViewBag.currentSearchBy = searchBy;
            ViewBag.currentSearchString = searchString;

            //Sort
            List<PersonResponse> sortedPersons = _personsService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();
            return View(sortedPersons); // Views/Persons/Index.cshtml
        }



        //Executes when the user clicks on "Create Person" hyperlink 
        [Route("[action]")]
        [HttpGet]
        public IActionResult Create()

        {
            List<CountryResponse> countries=  _countriesService.GetAllCountries();
            ViewBag.Countries = countries;
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries;

               ViewBag.Errors= ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();
                return View();
            }

            //Call to Service Method
            PersonResponse personResponse =_personsService.AddPerson(personAddRequest);

            //Navigate to Index() action method (it takes another get request to "persons/index"
            return RedirectToAction("Index","Persons");
        }
    }
}
