$(document).ready(function () {
    $('#service').change(function () {
        var serviceId = $(this).val();
        $('#master').empty().append('<option value="" disabled selected>Загрузка...</option>');
        $('#datetime').empty().append('<option value="" disabled selected>Выберите мастера сначала</option>');

        if (serviceId) {
            $.get('/User/GetMastersByService', { serviceId: serviceId }, function (masters) {
                $('#master').empty().append('<option value="" disabled selected>Выберите мастера</option>');
                masters.forEach(function (master) {
                    $('#master').append(`<option value="${master.id}">${master.name}</option>`);
                });
            });
        }
    });

    $('#master').change(function () {
        var masterId = $(this).val();
        $('#datetime').empty().append('<option value="" disabled selected>Загрузка...</option>');

        if (masterId) {
            $.get('/User/GetTimeSlots', { masterId: masterId }, function (timeslots) {
                $('#datetime').empty().append('<option value="" disabled selected>Выберите дату и время</option>');

                timeslots.forEach(function (timeslot) {
                    var date = new Date(timeslot.date);
                    if (!isNaN(date)) {
                        var formattedDate = date.toLocaleString('ru-RU', {
                            day: '2-digit',
                            month: '2-digit',
                            year: 'numeric',
                            hour: '2-digit',
                            minute: '2-digit'
                        });
                        $('#datetime').append(`<option value="${date.toISOString()}">${formattedDate}</option>`);
                    }
                });
            }).fail(function () {
                $('#datetime').empty().append('<option value="" disabled>Ошибка загрузки времени</option>');
            });
        }
    });
});