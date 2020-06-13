import React from 'react';


export default function ErrorForm(props){
    if(props.error.length > 0)
    {
        return(
            <p className="error-field">{props.error}</p>
        );
    }
    else
        return(
            <p></p>
        );
}