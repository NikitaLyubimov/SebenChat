import React, {Component} from 'react';

import "./LogInPage.scss";


export default class LogInPage extends Component{
    constructor(props){
        super(props);

        this.state = {
            userName: "",
            password: ""
        }
    }

    handleClick(){

    }

    handleChange(){

    }
    render(){
        return(
            <section>
               <div className="login-page">
                    <div className="form">
                        <p>Enter your account</p>
                        <form  className="login-form">
                            <input onChange={this.handleChange} name="userName" defaultValue={this.state.userName} type="text" placeholder="Username"/>
                            <input onChange={this.handleChange} name="password" defaultValue={this.state.password} type="password" placeholder="Password"/>
                            <button  onClick={this.handleClick} className="btn btn-success">Log In</button>
                        </form>
                        
                    </div>
                </div>
                
            </section>
        );
    }
}