using Application_Layer.DtoS;
using Application_Layer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation_Layer.Models;
using System.Diagnostics;
using System.Drawing;

namespace Presentation_Layer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ReservationService _reservationService;
        private readonly RoomService _roomService;
        private readonly MealPlanService _mealPlanService;

        public HomeController(ILogger<HomeController> logger, ReservationService reservationService, RoomService roomService, MealPlanService mealPlanService)
        {
            _logger = logger;
            _reservationService = reservationService;
            _roomService = roomService;
            _mealPlanService = mealPlanService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.RoomTypes = (await _roomService.GetRoomTypes()).Select(x => new SelectListItem
            {
                Text = x.RoomTypeName,
                Value = x.RoomTypeId.ToString()

            });
            ViewBag.MealPlans = (await _mealPlanService.GetMealPlans()).Select(x => new SelectListItem
            {
                Text = x.MealPlanName,
                Value = x.MealPlanId.ToString()

            });
            ViewBag.ChildrenNumberlist = Enumerable.Range(0, 10).Select(x => new SelectListItem
            {
                Text = x.ToString(),
                Value = x.ToString(),
                Selected = x == 0
            });

            ViewBag.Numberlist = Enumerable.Range(1, 10).Select(x => new SelectListItem
            {
                Text = x.ToString(),
                Value = x.ToString(),
                Selected = x == 1
            });

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateReservation(ReservationCreateDto reservationCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please, Enter Required data");
            }

            var reservationCreateResult = await _reservationService.CreateReservation(reservationCreateDto);
            if (reservationCreateResult.IsFailure)
            {
                return BadRequest(reservationCreateResult.Error);
            }

            return Json(new { success = true, reservation = reservationCreateResult.Value }); 
        }

    }
}
