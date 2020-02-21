import React, { useRef, useState } from 'react';
import axios from "axios";
import {TokenLocal} from "../TokenLocal/TokenLocal";
import "./css/styleLoginAndRegister.css";
import {Link,Redirect} from "react-router-dom";
import {ErrorList} from "./ErrorList";


export const Login = () => {


    let [isOk, setIsOk]= useState(false);
    let [ErrorMassage, setErrorMassage]= useState(null);
    let loginRef=useRef(null);
    let passRef=useRef(null);

    const handler=async ()=>{
        
        try{
            const login= loginRef.current.value;
            const password= passRef.current.value;
            console.log(login,password)
            var result= await axios.post("https://localhost:44302/account/getToken?Login="+login+"&Password="+password);
            TokenLocal.setIsAuth(true);
            TokenLocal.SetRefreshToken(result.data.refresh);
            TokenLocal.SetToken(result.data.jwt);
            setIsOk(true);
        }
        catch(e){
            setErrorMassage("Неправильный логин или пароль")
        }
    }

    if(isOk){
        return (
            <Redirect to="/chatReact"/>
        )
    }

    const ErrorMassageComp=ErrorMassage==null? null: 
    <ErrorList errors={[ErrorMassage]}/>

    return (
        <div className="form">
            {ErrorMassageComp}
            <div className="comp">
                <div className="dispInline">
                    <div className="signIn">
                        <Link to="/register">Регистрация</Link>
                    </div>
                    <div className="signUp activity">Войти</div>
                </div>
            
            
                <div >
                    <input type="text" placeholder="Введите логин" ref={loginRef}/>
                    <br />
                    <input type="password" placeholder="Введите пароль" ref={passRef}/>
                    <button className="button" onClick={handler}>Войти</button>	
                </div>
            </div>
        </div>
    );
}

