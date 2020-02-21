import React from 'react';

export const ErrorList=(props)=>{


    return(
        <div className={"errorList"}>
            <ul>
                {props.errors.map(el=><li>{el}</li>)}
            </ul>
        </div>
    )
}