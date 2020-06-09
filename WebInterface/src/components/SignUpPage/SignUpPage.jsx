import React from 'react';

import './SignUpPage.scss';

export default function SignUpPage(){
    return(
        <section>
           <div class="login-page">
            <div class="form">

            <form class="login-form">
            <input type="text" placeholder="First name"/>
            <input type="text" placeholder="Second name"/>
            <input type="text" placeholder="Username"/>
            <input type="password" placeholder="Password"/>
            <button className="btn btn-success">Sign Up</button>
            </form>
            </div>
            </div>
        </section>
    );
}