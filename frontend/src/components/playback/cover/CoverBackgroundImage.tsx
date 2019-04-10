import React, { ReactNode } from 'react';

interface CoverBackgroundImageProps {
    url: string | null;
    children: ReactNode;
}

const CoverBackgroundImage = (props: CoverBackgroundImageProps) => {
    const hasCoverImage = props.url != null;

    const className = "playback-background-cover" + (hasCoverImage ? " has-background" : null);
    const style = { backgroundImage: hasCoverImage ? `url(${props.url})` : undefined };

    return (
        <div className={className} style={style}>
            {props.children}
        </div>
    );
}

export default CoverBackgroundImage;