Object.defineProperty(HTMLElement.prototype, 'setAttribute2', {
    value: function(name, value) {
        this.setAttribute(name, value);
        return this;
    },
    writable: true,
    configurable: true
});

export function makeid(length) {
    let result = '';
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz';
    const charactersLength = characters.length;
    for (let i = 0; i < length; i++) {
        result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return result;
}

export function loadCss(path, scopeId) {
    const element = document.createElement('link')
        .setAttribute2('name', scopeId)
        .setAttribute2('rel', 'stylesheet')
        .setAttribute2('type', 'text/css')
        .setAttribute2('href', path);
    document.head.appendChild(element);
}
export function loadJs(path, scopeId, delegate) {
    const tag = document.createElement('script')
        .setAttribute2('name', scopeId)
        .setAttribute2('type', 'text/javascript')
        .setAttribute2('src', path);
    tag.onload = async function () {
        await delegate.invokeMethodAsync('OnLoad');
    }
    tag.onerror = async function () {
        await delegate.invokeMethodAsync('OnError');
    }
    document.body.appendChild(tag);
}
export function removeElements(scopeId) {
    Array.from(document.getElementsByName(scopeId)).forEach(x => x.remove());
}