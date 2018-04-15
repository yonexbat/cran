export class Tag {
    public id: number;
    public idTagType: TagType;
    public name: string;
    public description: string;
    public shortDescDe: string;
    public shortDescEn: string;
}

export enum TagType {
    Standard = 1,
    Warning = 2,
    Info = 3,
}
