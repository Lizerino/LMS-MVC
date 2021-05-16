    document.addEventListener('DOMContentLoaded', function () {

            var calendarEl = document.getElementById('calendar');
            var calendarEvents = $.get("/Activities/GetEvents",
                {
        Id: '@Model.First().moduleid'
                });
            var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth', events: [calendarEvents]
            });
            calendar.render();
        });