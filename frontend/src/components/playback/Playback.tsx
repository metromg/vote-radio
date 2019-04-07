import React, { Component } from 'react';

import './Playback.css';

class Playback extends Component {
    render() {
        const backgroundStyle = {
            backgroundImage: "url(https://images.pexels.com/photos/35646/pexels-photo.jpg?auto=compress&cs=tinysrgb&dpr=3&h=750&w=1260)"
        }

        return (
            <div className="playback">
                <div className="playback-mobile-cover" style={backgroundStyle}>
                    <button type="button" className="playback-playbutton">
                        <img src="/icons/icons8-play-filled-50.png" />
                    </button>
                    <div className="playback-song-description">
                        <div className="playback-subtitle">Interpret - Album</div>
                        <div className="playback-title">Song Titel</div>
                    </div>
                    <div className="playback-voting-info">5 Stimmen</div>
                    <div className="playback-progress"></div>
                    <div className="playback-desktop-cover" style={backgroundStyle}></div>
                </div>
            </div>
        );
    }
}

export default Playback;