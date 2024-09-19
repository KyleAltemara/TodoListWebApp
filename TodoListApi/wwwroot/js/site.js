const uri = '/api/todoitems';
let todos = [];

// Fetch and display all items
function getItems() {
    fetch(uri)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

// Add a new item
function addItem() {
    const addNameTextbox = document.getElementById('add-name');
    const name = addNameTextbox.value.trim();

    if (!name) {
        alert('Please enter a name for the to-do item.');
        return;
    }

    // Note: properties are converted to camelCase for JSON and JavaScript
    const item = {
        isComplete: false,
        name: name
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(() => {
            getItems();
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

// Delete an item by ID
function deleteItem(id) {
    const item = todos.find(item => item.id === id);
    const confirmation = confirm(`Are you sure you want to delete the to-do item: "${item.name}"?`);

    if (!confirmation) {
        return;
    }

    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            getItems();
        })
        .catch(error => console.error('Unable to delete item.', error));
}


// Display the edit form for a specific item
function displayEditForm(id) {
    // Note: properties are converted to camelCase for JSON and JavaScript
    const item = todos.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isComplete').checked = item.isComplete;
    document.getElementById('editForm').style.display = 'block';

    // Add event listeners to detect changes
    const editNameInput = document.getElementById('edit-name');
    const editIsCompleteCheckbox = document.getElementById('edit-isComplete');
    editNameInput.addEventListener('input', handleEditChange);
    editIsCompleteCheckbox.addEventListener('change', handleEditChange);
}


// Handle changes in the edit form
function handleEditChange() {
    displayMessage('The record hasn\'t been updated yet. Please submit the edited to-do.', 'info');
}

// Update an existing item
function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const name = document.getElementById('edit-name').value.trim();

    if (!name) {
        alert('Please enter a name for the to-do item.');
        return false;
    }

    // Note: properties are converted to camelCase for JSON and JavaScript
    const item = {
        id: parseInt(itemId, 10),
        isComplete: document.getElementById('edit-isComplete').checked,
        name: name
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            getItems();
            displayMessage('The to-do item has been successfully updated.', 'success', 3000); // Display message for 3 seconds
        })
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

// Function to display messages
function displayMessage(message, type, duration) {
    const messageContainer = document.getElementById('messageContainer');
    messageContainer.innerText = message;
    messageContainer.style.display = 'block';
    messageContainer.className = type; // Add a class for styling based on message type

    if (duration) {
        setTimeout(() => {
            messageContainer.style.display = 'none';
        }, duration);
    }
}

// Close the edit form
function closeInput() {
    document.getElementById('editForm').style.display = 'none';
    // Clear the message when the form is closed
    const messageContainer = document.getElementById('messageContainer');
    messageContainer.style.display = 'none';
}

// Display the count of items
function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'to-do' : 'to-dos';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

// Display all items in the table
function _displayItems(data) {
    const tBody = document.getElementById('todos');
    const table = document.getElementById('todoTable');
    tBody.innerHTML = '';

    _displayCount(data.length);

    if (data.length === 0) {
        table.style.display = 'none';
    } else {
        table.style.display = 'table';
        table.deleteTHead(); // Clear existing table header
        const thead = table.createTHead();
        const row = thead.insertRow();
        const headers = ['Is Complete?', 'Name', '', ''];
        headers.forEach(headerText => {
            const th = document.createElement('th');
            const text = document.createTextNode(headerText);
            th.appendChild(text);
            row.appendChild(th);
        });
    }

    const button = document.createElement('button');

    // Note: properties are converted to camelCase for JSON and JavaScript
    data.forEach(item => {
        let isCompleteCheckbox = document.createElement('input');
        isCompleteCheckbox.type = 'checkbox';
        isCompleteCheckbox.disabled = true;
        isCompleteCheckbox.checked = item.isComplete;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);
        editButton.setAttribute('aria-label', `Edit ${item.name}`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);
        deleteButton.setAttribute('aria-label', `Delete ${item.name}`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isCompleteCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.name);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    todos = data;
}