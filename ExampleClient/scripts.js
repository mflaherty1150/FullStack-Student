// A variable with our URL
const URL = "http://localhost:8080";

// A function to fetch our array of tasks (our data)
const getData = () => {
    fetch(URL)
        .then(response => response.json())
        .then(data => {
            console.log(data);
            data.forEach(task => appendItem(task));
        });
}

const appendItem = (task) => {
    let container = document.getElementById("task-container");
    let div = document.createElement("div");
    let header = document.createElement("h3");
    let p = document.createElement("p");

    div.id = task.Id; // C# way
    // div.id = task.id; // JavaScript way

    header.innerText = task.Title;
    p.innerText = task.Description;

    div.appendChild(header);
    div.appendChild(p);

    container.appendChild(div);
}

const handleSubmit = (event) => {
    // Stops the page from reloading
    event.preventDefault();
    console.log(event);

    // Take the HtmlFormElement and turn it into FormData
    const formData = new FormData(event.target);
    // Convert FormData into a simple object
    const obj = Object.fromEntries(formData);
    console.log(obj);

    fetch(URL, {
        method: "POST",
        mode: "cors",
        body: JSON.stringify(obj)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(response.status);
            }
            return response.json;
        })
        .then(data => {
            console.log(data);
        })
        .catch(err => console.error(err))
}