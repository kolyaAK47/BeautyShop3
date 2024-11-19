document.addEventListener("DOMContentLoaded", function () {
    var toggleButton = document.getElementById("toggleTableBtn");
    if (toggleButton) {
        toggleButton.onclick = function (event) {
            // Отменяем стандартное поведение ссылки
            event.preventDefault();

            // Показ/скрытие таблицы
            var table = document.getElementById("orderTable");
            if (table.style.display === "none" || table.style.display === "") {
                table.style.display = "block";
            } else {
                table.style.display = "none";
            }

            // AJAX-запрос на сервер для выполнения метода контроллера
            var url = '/Admin/GetAllOrders';
            fetch(url, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(response => response.json())
                .then(data => {
                    // После успешного выполнения запроса, обновите таблицу данными
                    console.log(data);
                    updateTable(data);
                })
                .catch(error => console.error('Error:', error));
        };
    } else {
        console.log("Button not found!");
    }
});

// Функция для обновления таблицы с новыми данными
function updateTable(data) {
    var tbody = document.querySelector("#table tbody");
    tbody.innerHTML = ""; // Очистить старые строки

    if (data.length === 0) {
        tbody.innerHTML = '<tr><td colspan="5" style="text-align: center;">Нет заказов</td></tr>';
    } else {
        data.forEach(order => {
            console.log(order);
            var row = document.createElement("tr");

            var orderIdCell = document.createElement("td");
            orderIdCell.textContent = order.orderId;
            row.appendChild(orderIdCell);
            console.log(order.Id);

            var userNameCell = document.createElement("td");
            userNameCell.textContent = order.userName;
            row.appendChild(userNameCell);

            var serviceCell = document.createElement("td");
            serviceCell.textContent = order.serviceName;
            row.appendChild(serviceCell);

            var masterCell = document.createElement("td");
            masterCell.textContent = order.masterName;
            row.appendChild(masterCell);

            var orderDateCell = document.createElement("td");
            
            /* Создаем объект Date из строки*/
            var orderDate = new Date(order.orderDate); // Преобразуем строку в объект Date
            if (!isNaN(orderDate.getTime())) {
                // Если дата валидная, выводим ее в нужном формате
                var formattedDate = orderDate.toLocaleString("ru-RU", {
                    year: "numeric",
                    month: "2-digit",
                    day: "2-digit",
                    hour: "2-digit",
                    minute: "2-digit"
                });
                orderDateCell.textContent = formattedDate;
            } else {
                orderDateCell.textContent = "Нет даты";
            }
            row.appendChild(orderDateCell);
            tbody.appendChild(row);
        });
    }
}