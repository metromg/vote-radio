import React from 'react';

interface SongDescriptionProps {
    title: string;
    album?: string;
    artist?: string;
}

const SongDescription = (props: SongDescriptionProps) => {
    const title = props.title;
    const subtitle = [props.artist, props.artist && props.album ? " - " : null, props.album]
        .filter(part => part != null)
        .join(" ");

    return (
        <div className="song-description">
            {
                props.artist || props.album
                    ?   <div className="subtitle" title={subtitle}>
                            {subtitle}
                        </div> 
                    : null
            }
            <div className="title" title={title}>
                {title}
            </div>
        </div>
    );
}

export default SongDescription;