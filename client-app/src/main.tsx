import { createRoot } from "react-dom/client";
import "semantic-ui-css/semantic.min.css";
import "react-calendar/dist/Calendar.css";
import "react-toastify/ReactToastify.css";
import "react-datepicker/dist/react-datepicker.css";
import "./app/layout/style.css";
import { store, StoreContext } from "./app/stores/store.ts";
import { RouterProvider } from "react-router-dom";
import { router } from "./app/router/Routes.tsx";

// In your main entry file (index.tsx or App.tsx)
export const suppressLibraryWarnings = () => {
  const originalError = console.error;

  console.error = (...args: any[]) => {
    if (
      /findDOMNode|defaultProps/.test(args[0]) ||
      /componentWillReceiveProps/.test(args[0])
    ) {
      return;
    }
    originalError.call(console, ...args);
  };
};
suppressLibraryWarnings();

createRoot(document.getElementById("root")!).render(
  <StoreContext.Provider value={store}>
    <RouterProvider router={router} />
  </StoreContext.Provider>
);
