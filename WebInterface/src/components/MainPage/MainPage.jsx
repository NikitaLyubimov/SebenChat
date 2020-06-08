import React from 'react';
import {Link} from 'react-router-dom';
import './MainPage.scss'


export default function MainPage()
{
    return(
        <section className="main-page">
            <div className="chat-btn">
                <p>Welcome to the Solyaris chat!</p>
            </div>
            <div className="buttons">
                <Link role="button" className="btn btn-primary sign-up" to="/sign-up">Sign Up</Link>
                <Link role="button" className="btn btn-primary log-in" to="/login">Log in</Link>
            </div>
        </section>
    );
}

