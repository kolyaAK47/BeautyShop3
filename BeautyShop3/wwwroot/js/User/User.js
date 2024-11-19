document.addEventListener("DOMContentLoaded", function () {
    var toggleButton = document.getElementById("toggleTableBtn");
    if (toggleButton) {
        toggleButton.onclick = function (event) {
            // �������� ����������� ��������� ������
            event.preventDefault();

            // �����/������� �������
            var table = document.getElementById("orderTable");
            if (table.style.display === "none" || table.style.display === "") {
                table.style.display = "block";
            } else {
                table.style.display = "none";
            }

            // AJAX-������ �� ������ ��� ���������� ������ �����������
            var url = '/Admin/GetAllOrders';
            fetch(url, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(response => response.json())
                .then(data => {
                    // ����� ��������� ���������� �������, �������� ������� �������
                    console.log(data);
                    updateTable(data);
                })
                .catch(error => console.error('Error:', error));
        };
    } else {
        console.log("Button not found!");
    }
});

// ������� ��� ���������� ������� � ������ �������
function updateTable(data) {
    var tbody = document.querySelector("#table tbody");
    tbody.innerHTML = ""; // �������� ������ ������

    if (data.length === 0) {
        tbody.innerHTML = '<tr><td colspan="5" style="text-align: center;">��� �������</td></tr>';
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
            
            /* ������� ������ Date �� ������*/
            var orderDate = new Date(order.orderDate); // ����������� ������ � ������ Date
            if (!isNaN(orderDate.getTime())) {
                // ���� ���� ��������, ������� �� � ������ �������
                var formattedDate = orderDate.toLocaleString("ru-RU", {
                    year: "numeric",
                    month: "2-digit",
                    day: "2-digit",
                    hour: "2-digit",
                    minute: "2-digit"
                });
                orderDateCell.textContent = formattedDate;
            } else {
                orderDateCell.textContent = "��� ����";
            }
            row.appendChild(orderDateCell);
            tbody.appendChild(row);
        });
    }
}