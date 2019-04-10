import React from 'react';

interface SongDescriptionProps {
    title: string;
    album?: string;
    artist?: string;
}

const SongDescription = (props: SongDescriptionProps) => {
    return (
        <div className="playback-song-description">
            {
                props.artist || props.album
                    ?   <div className="playback-subtitle">
                            {props.artist}
                            {props.artist && props.album ? " - " : null}
                            {props.album}
                        </div> 
                    : null
            }
            <div className="playback-title">
                {props.title}
            </div>
        </div>
    );
}

export default SongDescription;