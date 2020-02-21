import React, {useContext} from "react";
import {ChatContext} from "./Chat/ChatContext";
import "./css/style.css";

export const OptionMassage=(props)=>{

    let {InvokeHub}= useContext(ChatContext);
    const handlerChange=()=>{
        let content=prompt("Введите новый текст");
        InvokeHub("ChangeMassage",[props.id, content], "CHANGE_MASSAGE")

    }

    const handlerDelete=()=>{
        InvokeHub("DeleteMassage", props.id, "DELETE_MASSAGE")
    }

    return(
        <div className={"option"}>
            <span onClick={handlerChange}>🔧</span>
            <span onClick={handlerDelete}>&#10060;</span>
            
        </div>
    )

}