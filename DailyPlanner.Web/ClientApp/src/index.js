import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap/dist/css/bootstrap-theme.css';
import './index.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import { Link } from "react-router-dom";

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

window.getToken = () => window.token ? window.token : "";
window.isAuthorized = (responseStatus) => {
    if (responseStatus === "Unauthorized") {
        return <div>
            <div>
                You are {responseStatus.toLowerCase()}! Please <Link to="/account/login">login</Link> or <Link to="/account/register">register</Link> to continue :)
                </div>
        </div>;
    }
}


ReactDOM.render(
    <BrowserRouter basename={baseUrl}>
        <App />
    </BrowserRouter>,
    rootElement);

registerServiceWorker();
