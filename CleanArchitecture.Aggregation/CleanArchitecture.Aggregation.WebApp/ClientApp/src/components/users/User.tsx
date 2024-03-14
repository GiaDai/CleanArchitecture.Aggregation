import React from 'react';
import './User.css';
import { UserModel } from '../../@types/user';

export const User: React.FC<{ user: UserModel }> = ({ user }) => {
    return (
        <div className='media user-list'>
            <div className="media-left align-self-center">
                <img
                    className="rounded-circle"
                    src={`${process.env.PUBLIC_URL}/images/${user.gender == 'F' ? 'f.jpg' : 'm.png'
                        }`}
                />
                <h4>{user.name}</h4>
            </div>
            <div className="media-body"></div>
            <div className="media-right align-self-center">
                <div
                    className="btn btn-default"
                    style={{ background: user.point > 0 ? 'green' : '#6b456a' }}
                >
                    {user.showPoint ? user.point : 'Point'}
                </div>
            </div>
        </div>
    )
}