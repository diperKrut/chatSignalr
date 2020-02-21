import React from "react";
import {OptionMassage} from "./OptionMassage";


export const Massage=(props)=>{

    let isLoginUser=props.CurrentLogin===props.From;
    return(
        <div className={isLoginUser? "massageActivity": "massage"}>
            { isLoginUser?<OptionMassage id={props.id}/>: null }
            <b>{isLoginUser?"Вы": props.From}</b>
            <br></br>
             {props.Content}
        
        </div>
    )
}