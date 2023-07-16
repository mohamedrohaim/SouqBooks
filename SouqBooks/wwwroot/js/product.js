var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#productsTable').dataTable({
        "ajax": {
            "url":"/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            { "data": "coverType.name", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="d-flex justify-content-around">
                                             <a href="/Admin/Product/Upsert?id=${data}">
                                                <i class="fa-regular fa-pen-to-square" style="color: #12ce31;"></i>
                                            </a>
                                            <a href="/Admin/Product/Upsert?id=1" >
                                            <i class="fa-regular fa-trash-can" style="color: #f21f07;"></i>
                                            </a>
                        </div>
                      `
                },
                "width": "15%"
            },

            ]
    });
}