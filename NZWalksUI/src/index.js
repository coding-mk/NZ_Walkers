import React from "react";
import ReactDOM from "react-dom/client";

const el = document.getElementById("root");

const root = ReactDOM.createRoot(el);

function App() {
  let message = "Bye World!!!";
  if (Math.random() > 0.5) {
    message = "Hello World!!!";
  }
  return <h1>{message}</h1>;
}

root.render(<App />);
