(() => {
    'use strict';

    // Máscara dinâmica para telefones brasileiros: (00) 0000-0000 ou (00) 00000-0000.
    const formatPhone = (value) => {
        const digits = value.replace(/\D/g, '').slice(0, 11);
        if (!digits) return '';
        if (digits.length <= 2) return `(${digits}`;
        const ddd = digits.slice(0, 2);
        const number = digits.slice(2);
        if (digits.length <= 10) {
            if (number.length <= 4) return `(${ddd}) ${number}`;
            return `(${ddd}) ${number.slice(0, 4)}-${number.slice(4)}`;
        }
        if (number.length <= 5) return `(${ddd}) ${number}`;
        return `(${ddd}) ${number.slice(0, 5)}-${number.slice(5)}`;
    };

    document.querySelectorAll('[data-mask="telefone"]').forEach((input) => {
        input.setAttribute('inputmode', 'numeric');
        input.setAttribute('autocomplete', 'tel');
        input.setAttribute('maxlength', '15');
        input.addEventListener('input', () => {
            const start = input.selectionStart;
            const before = input.value;
            input.value = formatPhone(before);
            if (document.activeElement === input && start !== null) {
                const delta = input.value.length - before.length;
                const position = Math.max(0, Math.min(input.value.length, start + delta));
                input.setSelectionRange(position, position);
            }
        });
        input.value = formatPhone(input.value);
    });

    // Evita duplo envio acidental de formulários após a validação nativa.
    document.querySelectorAll('form[data-prevent-double-submit="true"]').forEach((form) => {
        form.addEventListener('submit', (event) => {
            if (!form.checkValidity()) return;
            const submit = form.querySelector('button[type="submit"], input[type="submit"]');
            if (submit?.dataset.submitted === 'true') {
                event.preventDefault();
                return;
            }
            if (submit) {
                submit.dataset.submitted = 'true';
                submit.disabled = true;
                submit.setAttribute('aria-busy', 'true');
            }
        });
    });
})();
