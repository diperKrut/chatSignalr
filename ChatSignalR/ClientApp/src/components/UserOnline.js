import React from "react";
import "./css/style.css"


export const UserOnline=(props)=>{
    return(
        <div className={"user"}>
            {props.IsOnline?<span className={"online"}>•</span>:<span className={"offline"}>•</span>} {props.Login}
            <hr></hr>
        </div>
    )
}