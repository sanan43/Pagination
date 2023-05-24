let dbServiceCount = $("#dbServiceCount").val()

$("#btnLoadMore").on("click", () => {
    let serviceCount = $("#services").children().length
    console.log(serviceCount)
    //    fetch("/Services/LoadMore?skip=" + serviceCount)
    //        .then(res => res.text())
    //        .then(data => {
    //            $("#services").append(data)
    //        })
    //})
    $.ajax("/Services/LoadMore", {
        method: "GET",
        data: {
            skip: serviceCount
        },
        success: (data) => {
            $("#services").append(data)
            serviceCount = $("#services").children().length
            if (serviceCount >= dbServiceCount) {
                $("#btnLoadMore").remove()
            }
        }
    })
})
    