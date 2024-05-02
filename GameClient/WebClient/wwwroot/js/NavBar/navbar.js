document.querySelector('.search input').addEventListener('focus', function() {
    this.classList.add('focused');
});

document.querySelector('.search input').addEventListener('blur', function() {
    this.value = '';
    this.classList.remove('focused');
});