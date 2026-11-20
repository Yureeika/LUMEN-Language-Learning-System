require.config({
    paths: {
        vs: "https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.45.0/min/vs"
    }
});

require(["vs/editor/editor.main"], function () {

    const editor = monaco.editor.create(
        document.getElementById("codeEditor"),
        {
            value: `name = input("Enter your name: ")
age = input("Enter your age: ")

print("Hello,", name)
print("You are", age, "years old.")`,
            language: "python",
            theme: "vs-dark",
            automaticLayout: true,
            minimap: {
                enabled: false
            }
        }
    );

    const runButton = document.getElementById("runCodeBtn");
    const inputBox = document.getElementById("programInput");
    const outputBox = document.getElementById("output");

    runButton.addEventListener("click", async function () {

        const code = editor.getValue();
        const input = inputBox.value;

        outputBox.textContent = "Running...";
        runButton.disabled = true;

        try {

            const response = await fetch("/VirtualLab/RunCode", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    Language: "python",
                    Code: code,
                    Input: input
                })
            });

            const result = await response.json();

            outputBox.textContent =
                result.output || "No output.";

        } catch (error) {

            outputBox.textContent =
                "Request failed: " + error.message;

        } finally {

            runButton.disabled = false;

        }
    });
});