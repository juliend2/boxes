const letterField = document.querySelector("#letter")
const numberField = document.querySelector("#number")

const cells = document.querySelectorAll(".js-cell")
cells.forEach(c => {
    c.addEventListener("click", (e) => {
        e.stopPropagation()
        letterField.value = String(e.target.dataset.letter).toUpperCase()
        numberField.value = e.target.dataset.number
    })
})