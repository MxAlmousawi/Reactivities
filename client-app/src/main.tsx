import { createRoot } from "react-dom/client";
import "semantic-ui-css/semantic.min.css";
import "react-calendar/dist/Calendar.css";
import "react-toastify/ReactToastify.css";
import "react-datepicker/dist/react-datepicker.css";
import "./app/layout/style.css";
import { store, StoreContext } from "./app/stores/store.ts";
import { RouterProvider } from "react-router-dom";
import { router } from "./app/router/Routes.tsx";

// Suppress specific React warnings
const suppressedWarnings = [
  "findDOMNode is deprecated",
  "Support for defaultProps will be removed",
];

const originalWarn = console.warn;
console.warn = (...args) => {
  if (suppressedWarnings.some((warning) => args[0]?.includes(warning))) {
    return;
  }
  console.log(suppressedWarnings);

  originalWarn(...args);
};

createRoot(document.getElementById("root")!).render(
  <StoreContext.Provider value={store}>
    <RouterProvider router={router} />
  </StoreContext.Provider>
);
