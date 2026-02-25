const uri = 'http://localhost:5203/todo'; 
let todos = [];
let pendingDeleteId = null;

// 页面加载时获取数据
document.addEventListener('DOMContentLoaded', () => {
    getTodos();
});

// 获取所有任务
async function getTodos() {
    try {
        const response = await fetch(uri);
        if (!response.ok) throw new Error('获取数据失败');
        todos = await response.json();
        displayTodos();
    } catch (error) {
        console.error('Error:', error);
        alert('无法加载任务列表，请确保后端服务已启动。');
    }
}

// 显示任务列表
function displayTodos() {
    const ul = document.getElementById('todo-list');
    ul.innerHTML = '';

    todos.forEach(todo => {
        const li = document.createElement('li');

        // 判断是否已完成
        if (todo.isComplete) {
            li.classList.add('completed');
        }

        // 生成 HTML 内容
        li.innerHTML = `
            <div class="todo-content">
                <input type="checkbox" ${todo.isComplete ? 'checked' : ''} onchange="toggleComplete(${todo.id}, this.checked)">
                <span class="todo-text">${escapeHtml(todo.name)}</span>
            </div>
            <div class="actions">
                <button class="btn-edit" onclick="editMode(${todo.id})">编辑</button>
                <button class="btn-delete" onclick="deleteTodo(${todo.id})">删除</button>
            </div>
        `;

        ul.appendChild(li);
    });
}

// 添加任务
async function addTodo() {
    const input = document.getElementById('todo-input');
    const name = input.value.trim();

    if (!name) {
        alert('请输入任务名称');
        return;
    }

    const todo = {
        name: name,
        isComplete: false
    };

    try {
        const response = await fetch(uri, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(todo)
        });

        if (response.ok) {
            input.value = ''; // 清空输入框
            getTodos(); // 刷新列表
        } else {
            alert('添加失败');
        }
    } catch (error) {
        console.error('Error:', error);
    }
}

// 打开删除确认弹窗
function deleteTodo(id) {
    pendingDeleteId = id;
    document.getElementById('delete-modal').classList.remove('d-none');
}

// 关闭删除确认弹窗
function closeDeleteModal() {
    pendingDeleteId = null;
    document.getElementById('delete-modal').classList.add('d-none');
}

// 确认删除任务
async function confirmDeleteTodo() {
    if (pendingDeleteId == null) return;

    try {
        const response = await fetch(`${uri}/${pendingDeleteId}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            closeDeleteModal();
            getTodos();
        } else {
            alert('删除失败');
        }
    } catch (error) {
        console.error('Error:', error);
    }
}

// 切换完成状态
async function toggleComplete(id, isComplete) {
    // 先在本地数组找到对应项，获取 name
    const todo = todos.find(t => t.id === id);
    if (!todo) return;

    const updatedTodo = {
        name: todo.name,
        isComplete: isComplete
    };

    try {
        const response = await fetch(`${uri}/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedTodo)
        });

        if (response.ok) {
            getTodos();
        } else {
            alert('更新状态失败');
        }
    } catch (error) {
        console.error('Error:', error);
    }
}

// 打开编辑弹窗
function editMode(id) {
    const todo = todos.find(t => t.id === id);
    if (!todo) return;

    document.getElementById('edit-todo-id').value = id;
    document.getElementById('edit-todo-input').value = todo.name;

    const modal = document.getElementById('edit-modal');
    modal.classList.remove('d-none');
    document.getElementById('edit-todo-input').focus();
}

// 关闭编辑弹窗
function closeEditModal() {
    document.getElementById('edit-todo-id').value = '';
    document.getElementById('edit-todo-input').value = '';
    document.getElementById('edit-modal').classList.add('d-none');
}

// 更新任务
async function updateTodo() {
    const id = Number(document.getElementById('edit-todo-id').value);
    const name = document.getElementById('edit-todo-input').value.trim();

    if (!name) {
        alert('名称不能为空');
        return;
    }

    // 获取当前状态
    const todo = todos.find(t => t.id == id);
    if (!todo) return;

    const updatedTodo = {
        name: name,
        isComplete: todo.isComplete
    };

    try {
        const response = await fetch(`${uri}/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedTodo)
        });

        if (response.ok) {
            closeEditModal(); // 关闭编辑弹窗
            getTodos();   // 刷新列表
        } else {
            alert('更新失败');
        }
    } catch (error) {
        console.error('Error:', error);
    }
}


// 点击遮罩关闭弹窗
document.addEventListener('click', (event) => {
    const editModal = document.getElementById('edit-modal');
    const deleteModal = document.getElementById('delete-modal');

    if (event.target === editModal) {
        closeEditModal();
    }

    if (event.target === deleteModal) {
        closeDeleteModal();
    }
});

// ESC 关闭弹窗
document.addEventListener('keydown', (event) => {
    if (event.key !== 'Escape') return;

    const editModal = document.getElementById('edit-modal');
    const deleteModal = document.getElementById('delete-modal');

    if (!editModal.classList.contains('d-none')) {
        closeEditModal();
    }

    if (!deleteModal.classList.contains('d-none')) {
        closeDeleteModal();
    }
});

// 防止 XSS 简单转义
function escapeHtml(text) {
    if (!text) return text;
    return text
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}