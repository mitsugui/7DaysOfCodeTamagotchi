export function initialize(dotNetObjectRef) {
    const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;

    if (!SpeechRecognition) {
        alert("Seu navegador não suporta reconhecimento de voz.");
        return;
    }

    const recognition = new SpeechRecognition();

    recognition.continuous = false; // Reconhece frases únicas
    recognition.interimResults = false; // Retorna apenas resultados finais
    recognition.lang = 'pt-BR'; // Define o idioma para Português do Brasil

    recognition.onstart = function () {
        console.log("Reconhecimento de voz iniciado.");
    };

    recognition.onresult = function (event) {
        if (event.results.length > 0) {
            const transcript = event.results[0][0].transcript;
            dotNetObjectRef.invokeMethodAsync('OnVoiceInputReceived', transcript);
        }
    };

    recognition.onerror = function (event) {
        console.error("Erro no reconhecimento de voz:", event.error);
        dotNetObjectRef.invokeMethodAsync('OnVoiceInputError', event.error);
    };

    recognition.onend = function () {
        console.log("Reconhecimento de voz encerrado.");
        dotNetObjectRef.invokeMethodAsync('OnVoiceInputEnded');
    };

    // Store the recognition instance for later use
    window.recognitionInstance = recognition;
}

export function startRecognition() {
    if (window.recognitionInstance) {
        window.recognitionInstance.start();
    }
}

export function stopRecognition() {
    if (window.recognitionInstance) {
        window.recognitionInstance.stop();
    }
}
