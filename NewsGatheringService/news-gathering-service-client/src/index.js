import React from "react";
import { render } from "react-dom";
import { BrowserRouter } from "react-router-dom";

import { Provider } from "react-redux";

import { App } from "./App";
import { CookiesProvider } from 'react-cookie';
import store from "./actions/store.js";
import 'bootstrap/dist/css/bootstrap.min.css';

import "./index.css";

render(

  <Provider store={store}>

    <BrowserRouter>
      <CookiesProvider>
        <App />
      </CookiesProvider>
    </BrowserRouter>
  </Provider>,
  document.getElementById("root")
);
/*render(
  <Provider store={store}>
    <App />
  </Provider>,
  document.getElementById("root")
);*/
