import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { HashRouter } from 'react-router-dom';
import { Auth0ProviderWithHistory } from './Auth0/AuthProvider';

ReactDOM.render(
  <React.StrictMode>
    <Auth0ProviderWithHistory>
      <HashRouter>
        <App />
      </HashRouter>
    </Auth0ProviderWithHistory>
  </React.StrictMode>,
  document.getElementById('root')
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
