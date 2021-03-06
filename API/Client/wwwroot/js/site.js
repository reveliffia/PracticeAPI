// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.

/*let animals = [
    { name: "dory", species: "fish", class: { name: "invertebrata" } },
    { name: "simba", species: "cat", class: { name: "mamalia" } },
    { name: "tori", species: "cat", class: { name: "mamalia" } },
    { name: "nemo", species: "fish", class: { name: "invertebrata" } },
    { name: "budi", species: "cat", class: { name: "mamalia" } }
]

let OnlyCat = [];

for (i = 0; i < animals.length; i++) {
    if (animals[i].species == "cat") {
        OnlyCat.push(animals[i]);       
    }    
}
console.log("OnlyCat", OnlyCat);

for (i = 0; i < animals.length; i++) {
    if (animals[i].species == "fish") {
        animals[i].class.name = "Non-Mamalia"
    }
}
console.log("Animals", animals);*/
/*
$.ajax({
    url: "https://swapi.dev/api/people/",
    success: function (result) {
        console.log(result.results);
        text = ""; 
        $.each(result.results, function (key,val) {
            text += <li>${val.hair_color}</li>;
        })
        console.log(text);
        $('#listSW').html(text);
    }
})*/


/*$.ajax({
    url: "https://pokeapi.co/api/v2/pokemon/",
    success: function (result) {
        console.log(result.results);
        text = "";
        $.each(result.results, function (key, val) {
            text += `<tr>
                <td>${key + 1}</td>
                <td>${val.name}</td>
                <td><button class="btn btn-primary" onclick="detailSW('${val.url}')" data-target="#modalSW" data-toggle="modal">Detail</button></td>
            </tr>`;
        })
        console.log(text);
        $('.tableSW').html(text);
    }
})*/

/*function detailSW(url) {
    $.ajax({
        url: url,
        success: function (result) {
            console.log(result);
            text = "";
            text = `
                    <table class="table table-hover">
                        <thead class="alert-warning">
                            <tr>
                                <td>Name</td>
                                <td>Weight</td>

                        </thead>
                        <tbody class="tableSW">
                             <tr>
                                    <td>${result.name}</td>
                                    <td>${result.weight}</td>                                    
                                </tr>
                        </tbody>
                    </table>
                   `
            $('.modal-body').html(text);
        }
    })
}*/
   

function detailSW(url) {
    ability = "";
    newtype = "";
    stat = "";
    move = "";

    $.ajax({
        url: url,
        success: function (result) {
            result.abilities.forEach(ab => {
                ability += `${ab.ability.name}\n`
            })
                  
            result.stats.forEach(s => {
                stat += `<tr>
                            <td>${s.stat.name}</td>
                            <td clas="col-8">
                            <div class="stat">
                              <div class="progress-bar progress-bar-striped bg-danger" role="progressbar" style="width: ${s.base_stat}%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100">${s.base_stat}%</div>
                            </div>                          
                                
                            </td>
                        </tr>`
            })
            result.types.forEach(t => {
                type = `${t.type.name}`
                if (type == "poison") {
                    newtype += `<span class="badge badge-pill badge-dark text-light">${type}</span>`
                }
                else if (type == "grass") {
                    newtype += `<span class="badge badge-pill badge-success">${type}</span>`
                }
                else if (type == "normal") {
                    newtype += `<span class="badge badge-pill badge-secondary">${type}</span>`
                }
                else if (type == "fire") {
                    newtype += `<span class="badge badge-pill badge-danger">${type}</span>`
                }
                else if (type == "water") {
                    newtype += `<span class="badge badge-pill badge-primary">${type}</span>`
                }
                else if (type == "bug") {
                    newtype += `<span class="badge badge-pill badge-info">${type}</span>`
                }                
                else if (type == "flying") {
                    newtype += `<span class="badge badge-pill badge-muted">${type}</span>`
                }
            })
            result.moves.forEach(m => {
                move += `<div class="badge badge-pill badge-primary text-dark">${m.move.name}\n</div>`
            })

            console.log(result);

            text = "";
            text = `
                    <div class="row">
                        <img src="${result.sprites.other.dream_world.front_default}" alt="" class="rounded-circle border img-fluid mx-auto d-block shadow-lg">
                    </div>
                    <div class="row justify-content-center my-3">
                        ${newtype}
                    </div>
                    <nav>
                        <div class="nav nav-tabs" id="nav-tab" role="tablist">
                        <a class="nav-item nav-link active" id="nav-home-tab" data-toggle="tab" href="#nav-home" role="tab" aria-controls="nav-home" aria-selected="true">Detail</a>
                        <a class="nav-item nav-link" id="nav-profile-tab" data-toggle="tab" href="#nav-profile" role="tab" aria-controls="nav-profile" aria-selected="false">Stat</a>
                        <a class="nav-item nav-link" id="nav-move-tab" data-toggle="tab" href="#nav-move" role="tab" aria-controls="nav-profile" aria-selected="false">Moves</a>
                        </div>
                    </nav>
                    <div class="tab-content" id="nav-tabContent">
                        <div class="tab-pane fade show active" id="nav-home" role="tabpanel" aria-labelledby="nav-home-tab">
                            <div class="row-auto border center bg-info text-light text-center">Detail</div>
                            <table class="table table-hover">
                                <tr>
                                    <td>Name</td>
                                    <td clas="col-8">: ${result.name}</td>
                                </tr>
                                <tr>
                                    <td>Weight</td>
                                    <td clas="col-8">: ${result.weight}</td>
                                </tr>
                                <tr>
                                    <td>height</td>
                                    <td clas="col-8">: ${result.height}</td>
                                </tr>
                                <tr>
                                    <td>Abilities</td>
                                    <td clas="col-8">: ${ability}</td>
                                </tr>
                            </table>
                        </div>
                        <div class="tab-pane fade" id="nav-profile" role="tabpanel" aria-labelledby="nav-profile-tab">
                            <div class="row-auto border center bg-info text-light text-center">Stat</div>
                            <table class="table table-hover">
                                ${stat}
                            </table>
                        </div>
                        <div class="tab-pane fade" id="nav-move" role="tabpanel" aria-labelledby="nav-move-tab">
                            <div class="row-auto border center bg-info text-light text-center">Moves</div>
                            <div class="row justify-content-center my-2">
                              ${move}
                            </div>
                        </div>
                    </div>

                   `
            $('.modal-body').html(text);
        }
    })
}

$(document).ready(function () {
    var no = 1;
    $('#datatableSW').DataTable({
        "ajax": {
            "url": "https://pokeapi.co/api/v2/pokemon/",
            "dataType": "json",
            "dataSrc": "results"
        },
        "columns": [
            {
                "data": null,
                render: function (data, type, row) {
                    return `<td>${no++}</td>`
                }
            },
            {
                "data": "name"
            },
            {
                "data": null,
                render: function (data, type, row) {
                    return `<button class="btn btn-primary" onclick="detailSW('${row['url']}')" data-target="#modalSW" data-toggle="modal">Action</button>`;
                }
            }
        ]
    });
});