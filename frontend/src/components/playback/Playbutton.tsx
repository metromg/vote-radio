import React, { MouseEvent } from 'react';

interface PlaybuttonProps {
    playing: boolean;

    onClick: (e: MouseEvent) => void;
}

const Playbutton = (props: PlaybuttonProps) => {
    return (
        <button type="button" className="playback-playbutton"
                onClick={e => props.onClick(e)}>
            {
                props.playing
                    ? <img src="/icons/icons8-stop-filled-50.png" />
                    : <img src="/icons/icons8-play-filled-50.png" />
            }
        </button>
    );
}

export default Playbutton;